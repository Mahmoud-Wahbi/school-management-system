# Improvements Roadmap

## 1. Caching

- [ ] Refactor to Decorator Pattern (CachedStudentService)
- [ ] Migrate from MemoryCache to Redis

## 2. Mapping

- [ ] Add AutoMapper to replace manual MapToDto in Services

## 3. Architecture

- [ ] Move GetQueryable out of Application layer (consider Specification Pattern)

## 4. Authorization

- [ ] Migrate from Service-level checks to Policy-based Authorization Handlers
- [ ] Create reusable IResourceAuthorizationService

## 5. Auditing

- [ ] Add Audit Log Table (track old/new values per change)
- [ ] Add Soft Delete (DeletedAt + DeletedBy on BaseEntity)

## 6. Logging

- [ ] Add Serilog DB sink to store logs in database
- [ ] Add automated Logs table cleanup (retain last 30 days)

## 7. Security

- [ ] Move SecretKey to Environment Variables (recommended for production)
- [ ] Consider Azure Key Vault for enterprise-level secret management
- [ ] Move Refresh Token from Body to HttpOnly Cookie
- [ ] Hash Refresh Tokens before storing in DB
- [ ] Add sliding expiration for Refresh Tokens
- [ ] Add account lockout after failed login attempts
- [ ] Use UserId claim instead of Identity.Name in rate limiter partition key

## 8. Future

- [ ] CORS (when Frontend is ready)

## 9. Monitoring & Alerts
- [ ] Add alert system for suspicious login attempts
- [ ] Add alert for repeated rate limit violations
- [ ] Add admin notifications for suspicious audit events
