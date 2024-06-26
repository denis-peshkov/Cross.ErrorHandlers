﻿namespace Cross.ErrorHandlers;

public enum ErrorCodeEnum
{
    AccessDenied = 1,
    AccountSelectionRequired = 2,
    AuthorizationPending = 3,
    ConsentRequired = 4,
    ExpiredToken = 5,
    InsufficientAccess = 6,
    InsufficientScope = 7,
    InteractionRequired = 8,
    InvalidClient = 9,
    InvalidGrant = 10,
    InvalidRequest = 11,
    InvalidRequestObject = 12,
    InvalidRequestUri = 13,
    InvalidScope = 14,
    InvalidToken = 15,
    LoginRequired = 16,
    MissingToken = 17,
    RegistrationNotSupported = 18,
    RequestNotSupported = 19,
    RequestUriNotSupported = 20,
    ServerError = 21,
    SlowDown = 22,
    TemporarilyUnavailable = 23,
    UnauthorizedClient = 24,
    UnsupportedGrantType = 25,
    UnsupportedResponseType = 26,
    UnsupportedTokenType = 27,

    InvalidOperation = 28,
    InvalidParameters = 29,
    InternalServerError = 30,
    NotFound = 31,
    Forbidden = 32,
    NotAuthorized = 33,
    BadRequest = 34,
    Conflict = 35,
    ImageNotFound = 36,
}
