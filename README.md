# SAP GUI Async

A WinUI 3 desktop application built in C# that provides a simplified, guided interface for interacting with SAP — designed for users who need to execute SAP transactions without deep SAP knowledge.

---

## The Problem

SAP is powerful but presents a significant barrier to non-technical users:

- Hundreds of transaction codes to memorise (MM01, ME21N, MIGO, etc.)
- No built-in validation before a transaction is committed — mistakes are costly to reverse
- Each transaction requires navigating complex, inconsistent screen layouts
- No way to execute multi-step workflows without switching between transactions manually

In a manufacturing environment, this means either restricting SAP access to a small number of trained users, or accepting the risk of data entry errors from less experienced staff.

---

## The Solution

SAP GUI Async replaces the raw SAP interface with a clean, purpose-built WinUI 3 desktop application. It exposes only the transactions your team actually uses, presents only the fields that are required, validates inputs before anything is committed to SAP, and supports multi-transactional workflows in a single guided process.

Users don't need to know a single transaction code.

---

## Key Features

- **Simplified transaction access** — common SAP transactions surfaced through a clean UI, no transaction codes required
- **Pre-commit validation** — inputs are validated before anything is sent to SAP, preventing costly data entry errors
- **Required fields only** — each operation presents only the fields relevant to that task, reducing confusion and mistakes
- **Multi-transactional workflows** — complex processes that would normally require multiple SAP transactions are combined into a single guided flow
- **Async operation** — all SAP calls run asynchronously, keeping the UI responsive during long-running RFC operations
- **Built for non-technical users** — designed from the ground up for shop floor and logistics staff, not SAP consultants

---

## Tech Stack

| Technology | Usage |
|---|---|
| C# / .NET | Core application logic |
| WinUI 3 | Desktop UI framework |
| SAP RFC SDK | SAP system integration via Remote Function Calls |
| Async/Await | Non-blocking SAP communication |

---

## Architecture

The application communicates with SAP via the **SAP RFC SDK**, making Remote Function Calls (RFCs) directly to the SAP backend. All RFC calls are executed asynchronously so the UI remains responsive while waiting for SAP responses.

```
┌─────────────────────┐         ┌─────────────────────┐
│   WinUI 3 Frontend  │         │     SAP Backend      │
│                     │         │                      │
│  - Guided UI        │──RFC───▶│  - Standard RFCs     │
│  - Validation layer │◀──────── │  - Custom BAPIs      │
│  - Workflow engine  │         │                      │
└─────────────────────┘         └─────────────────────┘
```

---

## Background

This application was built to solve a real problem in a live manufacturing environment at Kongsberg Automotive. SAP access was restricted because the learning curve was too steep for operational staff, creating bottlenecks where only a handful of people could perform critical data tasks.

By abstracting the SAP complexity behind a guided interface, the application opened up safe, validated SAP access to a much wider group of users — reducing bottlenecks and improving data accuracy across the operation.

---

## Status

Under active development. Current focus areas:

- [ ] Expanding the range of supported transactions
- [ ] Role-based access control (restrict which transactions each user can see)
- [ ] Audit logging of all committed transactions
- [ ] Offline validation mode (validate without a live SAP connection)

---

## Author

**Matthew Walker** — Systems & Application Developer  
[LinkedIn](https://linkedin.com/in/matthew-walker-1b740418b) · [GitHub](https://github.com/TheMatthewWalker)