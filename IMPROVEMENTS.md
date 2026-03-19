# Improvements Roadmap

## 1. Caching
- [ ] Refactor to Decorator Pattern (CachedStudentService)
- [ ] Migrate from MemoryCache to Redis

## 2. Ownership / Authorization
- [ ] Migrate from Service-level checks to Policy-based Authorization Handlers
- [ ] Create reusable IResourceAuthorizationService

## 3. Mapping
- [ ] Add AutoMapper to replace manual MapToDto in Services

## 4. Architecture
- [ ] Move GetQueryable out of Application layer (consider Specification Pattern)

## 5. Future
- [ ] CORS (when Frontend is ready)
- [ ] Rate Limiting
- [ ] Refresh Tokens
- [ ] Auditing