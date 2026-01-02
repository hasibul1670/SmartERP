# Data Flow: Postman / Web → Database

## 1. Client (Postman / Web)
- Sends HTTP request (JSON payload)
- Example: `POST /api/product-categories`

---

## 2. API Layer (Presentation)
- ASP.NET Core receives the request
- JSON is mapped to a Request Model
- Endpoint/Controller sends a **Command** to Application layer

**Responsibility:**
- HTTP handling
- Request/Response mapping
- No business logic

---

## 3. Application Layer
- Command Handler executes
- Validates data
- Applies business rules
- Creates Domain Entity
- Calls Repository **interface**

**Responsibility:**
- Use-case logic
- Orchestration
- Depends on interfaces only

---

## 4. Infrastructure Layer
- Repository implementation is executed
- Uses EF Core `DbContext`
- Calls `SaveChangesAsync()`

**Responsibility:**
- Database access
- External services
- Technical details

---

## 5. Database
- EF Core generates SQL
- SQL is executed on the database
- Data is persisted

---

## Short Flow Summary

```text
Postman / Web
    ↓
API (Endpoint / Controller)
    ↓
Application (Command Handler)
    ↓
Infrastructure (Repository + DbContext)
    ↓
Database
