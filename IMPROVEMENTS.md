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

## 6. Future
- [ ] CORS (when Frontend is ready)
- [ ] Rate Limiting
- [ ] Refresh Tokens
- [ ] Move Refresh Token from Body to HttpOnly Cookie
