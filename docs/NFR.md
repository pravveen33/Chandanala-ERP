# NFR.md
- **Availability:** 99.99% for authentication services.
- **Latency:** Authentication handshakes must resolve < 150ms.
- **Traceability:** Every identity event (Login, PasswordChange, TokenRefresh) must emit a Domain Event.
- **Security:** Zero-trust architecture. RBAC/ABAC enforced at the API Gateway level.
- **Audit:** Immutable log of all security-related claims and access requests.
