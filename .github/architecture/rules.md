CHANDANALA CORE™ Architecture Rules

1. UI must never access DbContext directly.

2. UI must only call Services.

3. Services must not reference Razor Components.

4. Repository layer only accesses EF Core.

5. Domain models must not reference UI.

6. Circular project references are prohibited.

7. Business rules belong in Services.

8. Configuration must come from appsettings or environment variables.

9. No hardcoded secrets.

10. Every new module requires:
   - Unit Tests
   - Audit Logging
   - Authorization
   - Documentation