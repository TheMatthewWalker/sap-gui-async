using costing_tool;
using costing_tool.pages;
using SAPFunctionsOCX;
using System;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;


public class SapController // This class handles all SAP related functions and queries
{

    // Function to log in to SAP system
    public Task<bool> LoginAsync(
                            string system,
                            string client,
                            string systemId,
                            string user,
                            string password)
    {
        return App.SapWorker.InvokeAsync(sap =>
        {
            return SapLogin(system, client, systemId, user, password, sap);
        });
    }

        public bool SapLogin(
                            string system,
                            string client,
                            string systemId,
                            string user,
                            string password,
                            SAPFunctions sap)
        {
        try
        {
            dynamic conn = sap.Connection;
            conn.System = system;
            conn.Client = client;
            conn.SystemID = systemId;
            conn.User = user;
            conn.Password = password;
            return (bool)conn.Logon(0, true);
        }
        catch (COMException ex)
        {
            System.Diagnostics.Debug.WriteLine($"SAP login failed (HRESULT {ex.ErrorCode}): {ex.Message}");
            return false;
        }
        }



    // Functions to use Warehouse transactions
    public Task<TOCreateResult> CreateTransferOrderAsync(string plant, string sloc, string warehouse, string binType, string bin, string material, string batch, decimal qty, string destBin, string destBinType, string category, string special, string specialNumber)
    {
        return App.SapWorker.InvokeAsync(sap =>
        {
            return CreateTransferOrder(plant, sloc, warehouse, binType, bin, material, batch, qty, destBin, destBinType, category, special, specialNumber, sap);
        });
    }

    public TOCreateResult CreateTransferOrder(string plant, string sloc, string warehouse, string binType, string bin, string material, string batch, decimal qty, string destBin, string destBinType, string category, string special, string specialNumber, SAPFunctions sapFuncs)
    {
        try
        {
            dynamic func = sapFuncs.Add("L_TO_CREATE_SINGLE");

            if (func == null)
            {
                return new TOCreateResult
                {
                    TransferOrderNumber = "",
                    Message = "",
                    Error = "Function module not found"
                };
            }

            // Set the import parameters for the function module
            func.Exports("I_LGNUM").Value = warehouse;         // Warehouse number
            func.Exports("I_WERKS").Value = plant;             // Plant
            func.Exports("I_LGORT").Value = sloc;              // Storage Location
            func.Exports("I_SQUIT").Value = "X";              // Immediate Confirmation

            func.Exports("I_BWLVS").Value = "999";         // Movement Type

            func.Exports("I_MATNR").Value = material;          // Material
            func.Exports("I_ANFME").Value = qty;               // Quantity
            func.Exports("I_CHARG").Value = batch ?? "";       // Batch number
            func.Exports("I_ZEUGN").Value = batch ?? "";       // Certificate number

            func.Exports("I_VLTYP").Value = binType;         // Storage bin type
            func.Exports("I_VLPLA").Value = bin;             // Storage bin

            func.Exports("I_BESTQ").Value = category ?? "";        // Stock Category (Blocked / Quality)
            func.Exports("I_SOBKZ").Value = special ?? "";         // Special Stock Indicator
            func.Exports("I_SONUM").Value = specialNumber ?? "";   // Special Stock Number

            func.Exports("I_NLPLA").Value = destBin;         // Destination bin
            func.Exports("I_NLTYP").Value = destBinType;     // Destination bin type

            // Execute BAPI
            bool success = func.Call;
            if (!success)
                return new TOCreateResult
                {
                    TransferOrderNumber = "",
                    Message = "",
                    Error = Convert.ToString(func.Exception)
                };

            // Read return messages
            var returnTable = func.Tables.Item("RETURN");
            string messages = "";
            string errorMsg = "";

            if (returnTable != null)
            {
                int rc = 0;
                try { rc = (int)returnTable.Rows.Count; } catch { rc = 0; }

                for (int i = 1; i <= rc; i++)
                {
                    var row = returnTable.Rows.Item(i);
                    string type = Convert.ToString(row["TYPE"] ?? "");
                    string msg = Convert.ToString(row["MESSAGE"] ?? "");
                    messages += $"{type}: {msg}\n";
                    if (type == "E" || type == "A")
                        errorMsg = msg;
                }
            }

            // Transfer Order number (if created)
            string toNumber = Convert.ToString(func.Imports.Item("E_TANUM").Value);


            return new TOCreateResult
            {
                TransferOrderNumber = toNumber,
                Message = messages.Trim(),
                Error = errorMsg
            };

        }
        catch (Exception ex)
        {
            string inner = ex.InnerException?.Message ?? "null";
            string trace = ex.StackTrace ?? "null";

            return new TOCreateResult
            {
                TransferOrderNumber = "",
                Message = "",
                Error = inner
            };
        }
    }






    // Functions to retreieve cost sheet data
    public Task<CostSheetResult> CostSheetAsync(string[][] materialFilters)
    {
        return App.SapWorker.InvokeAsync(sap =>
        {
            return CostSheet(materialFilters, sap);
        });
    }
    public CostSheetResult CostSheet(string[][] materialFilters, SAPFunctions sapFuncs)
    {
        var result = new CostSheetResult();


        // Initialize SAP Functions
        // ==========================
        // dynamic sapFuncs = App.SapFuncs;
        // dynamic conn = sapFuncs.Connection;
        // This is done in seperate STA thread now added as argument

        dynamic func = sapFuncs.Add("ZRFC_READ_TABLES");
        if (func == null)
            throw new Exception("Failed to add ZRFC_READ_TABLES function.");


        // Define parameters
        // ======================
        //int rowCount = 1000;
        string delimiter = ";";

        //func.exports("ROWCOUNT").Value = rowCount;
        func.exports("DELIMITER").Value = delimiter;
        func.exports("NO_DATA").Value = " ";

        var qt = func.Tables("QUERY_TABLES"); qt.Freetable();
        var qf = func.Tables.Item("query_FIELDS"); qf.Freetable();
        var jf = func.Tables.Item("join_FIELDS"); jf.Freetable();
        var wc = func.Tables.Item("where_clause"); wc.Freetable();
        var op = func.Tables.Item("value_list"); op.Freetable();


        // Set table parameters
        // ======================
        string[] tablesToRead = new[] { 
                                        "ZCOST_INFO3", 
                                        "PATN",
                                        "ZCOST_SHEET" 
                                      };

        foreach (var name in tablesToRead)
        {
            var row = qt.Rows.Add();
            row["TABNAME"] = name;
        }


        // Set query fields
        // ==================
        string[][] fieldsToRead = [ 
                                    ["ZCOST_INFO3","MATNR"],
                                    ["ZCOST_INFO3","WERKS"],
                                    ["ZCOST_INFO3","KADAT"],
                                    ["ZCOST_INFO3","BIDAT"],
                                    ["ZCOST_INFO3","PRCTR"],
                                    ["ZCOST_INFO3", "BUKRS"],
                                    ["ZCOST_INFO3", "PATNR"],
                                    ["ZCOST_INFO3", "KST001"],
                                    ["ZCOST_INFO3", "KST008"],
                                    ["ZCOST_INFO3", "KST017"],
                                    ["ZCOST_INFO3", "KST002"],
                                    ["ZCOST_INFO3", "KST004"],
                                    ["ZCOST_INFO3", "KST019"],
                                    ["ZCOST_INFO3", "KST006"],
                                    ["ZCOST_INFO3", "KST033"],
                                    ["ZCOST_INFO3", "LOSGR"],
                                    ["ZCOST_INFO3", "MEINS"],
                                    ["ZCOST_INFO3", "FEH_STA"],
                                    ["PATN", "WERK"],
                                    ["ZCOST_SHEET", "VALID_FROM"],
                                    ["ZCOST_SHEET", "VALID_TO"],
                                    ["ZCOST_SHEET", "OH_PCT"],
                                    ["ZCOST_SHEET", "IC_MARK_UP"]   
                                  ];
        
        foreach (var name in fieldsToRead)
        {
            var row = qf.Rows.Add();
            row["TABNAME"] = name[0];
            row["FIELDNAME"] = name[1];
        }


        // Set join fields
        // ==================
        string[][] joinFields = [ 
                                    [ "ZCOST_INFO3", "PATNR", "PATN", "PATNR" ],
                                    [ "PATN", "WERK", "ZCOST_SHEET", "WERKS" ]    
                                ];

        foreach (var name in joinFields)
        {
            var row = jf.Rows.Add();
            row["TAB_FROM"] = name[0];
            row["FLD_FROM"] = name[1];
            row["TAB_TO"] = name[2];
            row["FLD_TO"] = name[3];
        }


        // Set where clause
        // ==================
        string[][] whereFields = [  
                                    ["ZCOST_INFO3", "WERKS", "EQ '3012'"], 
                                    ["ZCOST_INFO3", "FEH_STA", "EQ 'FR'"],
                                    ["ZCOST_INFO3", "MATNR", "IN opt"],
                                    ["ZCOST_INFO3", "KADAT", "EQ '01.01.2026'"],
                                    //["ZCOST_INFO3", "BIDAT", $"EQ '{GetEndOfCurrentMonth()}'"]
                                 ];

        foreach (var name in whereFields)
        {
            var row = wc.Rows.Add();
            row["TEXT"] = name[0] + "~" + name[1] + " " + name[2];
        }


        // Set option text
        // ==================
        foreach (var name in materialFilters)
        {
            var row = op.Rows.Add();
            row["TABNAME"] = "ZCOST_INFO3";
            row["FIELDNAME"] = "MATNR";
            row["SIGN"] = "I";
            row["OPTION"] = "";
            row["LOW"] = SapN2C(name[0],18);
            row["HIGH"] = "";
        }


        // Create objects to debug sap function data
        // ====================================
        object[,] qtdata = qt.Data;
        object[,] qfdata = qf.Data;
        object[,] jfdata = jf.Data;
        object[,] wcdata = wc.Data;
        object[,] opdata = op.Data;


        // Call the function
        // ==================
        bool sapExec;
        try
        {
            sapExec = func.Call;
        }
        catch (COMException ex)
        {
            throw new Exception($"SAP call failed (HRESULT {ex.ErrorCode})", ex);
        }

        if (!sapExec)
            throw new Exception("SAP RFC_READ_TABLE call failed.");


        // Process returned data
        var dataTable = func.tables.Item("data_display");
        object[,] data = dataTable.Data;
        int header = 0;

        foreach (var dataRow in dataTable.Rows)
        {
            header += 1;
            if (header == 1) 
                continue;

            string rowStr = Convert.ToString(dataRow["WA"]) ?? string.Empty;
            var cols = rowStr.Split(delimiter);

            if (!(cols.ElementAtOrDefault(18)?.Trim() == "3012"))
                continue;

            var masterRecord = new TableCOST
            {
                Material = long.TryParse(cols.ElementAtOrDefault(0)?.Trim(), out _)
                    ? cols.ElementAtOrDefault(0)?.Trim().TrimStart('0') ?? ""
                    : cols.ElementAtOrDefault(0)?.Trim() ?? "",
                CostingDate = cols.ElementAtOrDefault(2)?.Trim() ?? "",
                ProfitCenter = cols.ElementAtOrDefault(4)?.Trim() ?? "",
                DirectMaterial = decimal.TryParse(cols.ElementAtOrDefault(7)?.Trim(), out decimal kst001Val) ? Math.Round(kst001Val / 1000,2 ) : 0m,
                DirectLabour = decimal.TryParse(cols.ElementAtOrDefault(8)?.Trim(), out decimal kst008Val) ? Math.Round(kst008Val / 1000, 2) : 0m,
                VariableProductionCost = decimal.TryParse(cols.ElementAtOrDefault(9)?.Trim(), out decimal kst017Val) ? Math.Round(kst017Val / 1000, 2) : 0m,
                InboundFreight = decimal.TryParse(cols.ElementAtOrDefault(10)?.Trim(), out decimal kst002Val) ? Math.Round(kst002Val / 1000, 2) : 0m,
                OutboundFreight = decimal.TryParse(cols.ElementAtOrDefault(11)?.Trim(), out decimal kst004Val) ? Math.Round(kst004Val / 1000, 2) : 0m,
                Scrap = decimal.TryParse(cols.ElementAtOrDefault(12)?.Trim(), out decimal kst019Val) ? Math.Round(kst019Val / 1000, 2) : 0m,
                Unit = cols.ElementAtOrDefault(16)?.Trim() ?? "",
                Packaging = 0,
                ExtrusionLabour = 0
            };

            foreach (var name in materialFilters)
                {
                    if (name[0].Trim() == masterRecord.Material.Trim())
                    {
                        masterRecord.Customs = name[2] switch
                        {
                            "EXW" => 0,
                            "FCA" => 25,
                            "DAP" => 25,
                            "DDP" => 80,
                            _ => 0
                        };
                        masterRecord.Packaging = CalculatePackagingCost(name[0], int.TryParse(name[1], out int packVal) ? packVal : 0);
                        masterRecord.OutboundFreight = CalculateOutboundFreight(name[0], int.TryParse(name[1], out int packVal2) ? packVal2 : 0, name[2], name[3]);
                    }
                }


            masterRecord.Total = Math.Round(
                        masterRecord.DirectMaterial
                      + masterRecord.DirectLabour
                      + masterRecord.VariableProductionCost
                      + masterRecord.InboundFreight
                      + masterRecord.OutboundFreight
                      + masterRecord.Scrap
                      + masterRecord.Packaging
                      + masterRecord.Customs
                      , 2);


            var initialRecord = new InitialCost
            {
                Material = masterRecord.Material,
                CostingDate = masterRecord.CostingDate,
                ProfitCenter = masterRecord.ProfitCenter,
                Total = masterRecord.Total,
                Unit = masterRecord.Unit
            };

            result.AllData.Add(masterRecord);
            result.QuickData.Add(initialRecord);
        }

        return result;
    }
    public decimal CalculatePackagingCost(string matnr, int volume)
    {
        decimal packagingCost;
        packagingCost = 0;
                
        return packagingCost;
    }
    public decimal CalculateOutboundFreight(string matnr, int volume, string inco, string country)
    {
        decimal freightCost;
        freightCost = 0;

        string tmpText;

        tmpText = inco + " " + country;

        return freightCost;
    }






    // Useful functions to simplify SAP data handling
    public string SapN2C(string value, int columnLength)
    {
        string material = value.Trim() ?? "";

        // Check if numeric
        if (long.TryParse(material, out long numericValue))
        {
            // Pad with leading zeros to length 18
            string padded = numericValue.ToString().PadLeft(columnLength, '0');
            return padded;
        }
        else
        {
            // Not numeric – leave as-is or show a message
            return material;
        }

    }
    public string GetStartOfCurrentMonth()
    {
        var now = DateTime.Now;
        var startOfMonth = new DateTime(now.Year, now.Month, 1);

        return startOfMonth.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
    }
    public string GetEndOfCurrentMonth()
    {
        var now = DateTime.Now;
        var endOfMonth = new DateTime(now.Year, now.Month, 1)
            .AddMonths(1)
            .AddDays(-1);

        return endOfMonth.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
    }






}
