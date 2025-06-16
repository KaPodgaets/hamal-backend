---
id: ARCH-feature-authentication
title: "Feature: User Authentication and Authorization"
type: feature
layer: application
owner: "@dev-team"
version: v1
status: current
created: 2025-06-16
updated: 2025-06-16
tags: [feature, security, authentication, authorization, jwt]
depends_on: [ARCH-domain-entities, ARCH-api-layer, ARCH-infrastructure-layer]
referenced_by: []
---

## Context

This feature provides secure access to the API using role-based access control (RBAC). It ensures that only authenticated users can access the system and that they can only perform actions permitted by their assigned role.

## Structure

- **API Layer**:
  - `AuthController`: Contains the `login` endpoint.
  - Authentication middleware (`AddAuthentication` with `JwtBearerDefaults`) is configured to validate tokens.
  - `[Authorize(Roles = "...")]` attributes are used on controllers and actions to enforce authorization.
- **Infrastructure Layer**: A JWT generation service (`JwtTokenGenerator`) creates and signs tokens for authenticated users.

## Behavior

1.  A user submits their credentials to the `POST /api/auth/login` endpoint.
2.  The system validates the credentials against the stored user data.
3.  If valid, the JWT service generates a signed, stateless JWT access token containing the user's ID and role. The token has a defined expiration time.
4.  The client includes this token in the `Authorization` header of subsequent requests (e.g., `Bearer <token>`).
5.  The authentication middleware on the server validates the token on each request. If the token is valid and not expired, the user's identity and roles are established for the request context, allowing authorization checks to pass or fail accordingly.
6.  The system does not use refresh tokens; the user must re-authenticate after the access token expires.
