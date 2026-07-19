# QUALITY_GATES.md
Every change to CHANDANALA CORE™ must pass these gates before production release:

1. **Architecture Compliance:** Code must adhere to Clean Architecture boundaries (No Domain leakage into Infrastructure).
2. **Build Success:** Zero build errors or warnings.
3. **Automated Testing:** 
   - Unit Test coverage > 80%.
   - Integration tests mandatory for all Domain logic.
4. **Security Audit:** Zero critical/high vulnerability findings via automated scanning.
5. **Static Analysis:** Zero violations of defined coding standards.
6. **Performance:** Critical path latency < 200ms under simulated load.
7. **Documentation:** ADRs and Domain Model updates must accompany any breaking API change.
8. **Human Approval:** Principal Engineer review required for all changes to the Deterministic Core.
