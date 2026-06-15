-- TABLES CREATION --
USE hangmanDB;
GO

-- ==
-- USER
-- ==
CREATE TABLE [User](
    userId		INT 		NOT NULL IDENTITY(1,1),
    roleId		TINYINT		NOT NULL,
    fullName	VARCHAR(100)	NOT NULL,
    birthDate	DATE		NOT NULL,
    phoneNumber	VARCHAR(20)	NOT NULL,
    username 	VARCHAR(50)	NOT NULL,
    email		VARCHAR(100)	NOT NULL,
    pwdHash		VARCHAR(256)	NOT NULL,
    salt		VARCHAR(64)	NOT NULL,
    isActive	BIT		NOT NULL,
    createdAt	DATETIME 	NOT NULL DEFAULT GETUTCDATE(),
    updatedAT	DATETIME	NULL
);
GO

CREATE TABLE UserRole (
    userRoleId  INT         NOT NULL IDENTITY(1,1),
    userId      INT         NOT NULL,
    roleId      TINYINT     NOT NULL,
    grantedAt   DATETIME    NOT NULL DEFAULT GETUTCDATE()
);
GO

CREATE TABLE [Role] (
    roleId      TINYINT     NOT NULL IDENTITY(1,1),
    roleName    VARCHAR(50) NOT NULL
);
GO

CREATE TABLE UserSession (
    sessionId   INT         IDENTITY(1,1),
    userId      INT         NOT NULL,
    token       VARCHAR(128)NOT NULL,
    startedAt   DATETIME    NOT NULL DEFAULT GETUTCDATE(),
    endedAt     DATETIME    NULL
);
GO


CREATE TABLE PlayerProfile (
    profileId	INT		NOT NULL IDENTITY(1,1),
    userId		INT		NOT NULL,
    avatarURL	VARCHAR(255)	NULL,
    bio		VARCHAR(255)	NULL,
    theme		VARCHAR(50)	NULL DEFAULT 'default',
    updatedAt	DATETIME	NULL
);
GO


CREATE TABLE PlayerStats(
    statsId     INT         NOT NULL IDENTITY(1,1),
    userId      INT         NOT NULL,
    gamesPlayed INT         NOT NULL DEFAULT 0,
    gamesWon    INT         NOT NULL DEFAULT 0,
    totalScore  INT         NOT NULL DEFAULT 0,
    winRate     DECIMAL(5,2) NOT NULL DEFAULT 0.00,
    updatedAt   DATETIME    NULL
);
GO


CREATE TABLE PasswordRecoveryToken(
    tokenId 	INT		NOT NULL IDENTITY(1,1),
    userId		INT		NOT NULL,
    token		VARCHAR(128)	NOT NULL,
    expiresAt	DATETIME	NOT NULL,
    isUsed		BIT		NOT NULL DEFAULT 0,
    createdAt	DATETIME	NOT NULL DEFAULT GETUTCDATE()
);
GO

-- ==
-- SOCIAL
-- ==
CREATE TABLE Friendship (
    friendshipId    INT         NOT NULL IDENTITY(1,1),
    userId          INT         NOT NULL,
    friendId        INT         NOT NULL,
    since           DATETIME    NOT NULL DEFAULT GETUTCDATE()
);
GO

CREATE TABLE FriendRequest (
    requestId       INT             NOT NULL IDENTITY(1,1),
    senderId        INT             NOT NULL,
    receiverId      INT             NOT NULL,
    [status]          VARCHAR(10)     NOT NULL DEFAULT 'PENDING',
    sentAt          DATETIME        NOT NULL DEFAULT GETUTCDATE(),
    resolvedAt      DATETIME        NULL
);
GO

-- ==
-- MATCH
-- ==
CREATE TABLE [Match] (
    matchId         INT             NOT NULL IDENTITY(1,1),
    hostId          INT             NOT NULL,
    wordId          INT 	    NULL,
    [status]        VARCHAR(15)     NOT NULL DEFAULT 'WAITING',
    maxPlayers      INT             NOT NULL DEFAULT 2,
    isLocalNetwork  BIT             NOT NULL DEFAULT 0,
    createdAt       DATETIME        NOT NULL DEFAULT GETUTCDATE(),
    finishedAt      DATETIME        NULL
);
GO

CREATE TABLE PlayerInMatch (
    playerInMatchId INT         NOT NULL IDENTITY(1,1),
    matchId         INT         NOT NULL,
    userId          INT         NOT NULL,
    [role]            VARCHAR(10) NOT NULL DEFAULT 'GUESSER',
    score           INT         NOT NULL DEFAULT 0,
    isKicked        BIT         NOT NULL DEFAULT 0,
    joinedAt        DATETIME    NOT NULL DEFAULT GETUTCDATE()
);
GO


CREATE TABLE Word (
    wordId      INT             NOT NULL IDENTITY(1,1),
    word        VARCHAR(100)    NOT NULL,
    category    VARCHAR(50)     NOT NULL,
    difficulty  VARCHAR(10)     NOT NULL,
    language    VARCHAR(10)     NOT NULL DEFAULT 'en',
    addedBy     INT             NOT NULL,
    createdAt   DATETIME        NOT NULL DEFAULT GETUTCDATE()
);
GO

CREATE TABLE GameSession (
    sessionId       INT             NOT NULL IDENTITY(1,1),
    matchId         INT             NOT NULL,
    wordId          INT             NOT NULL,
    winnerId	    INT 	    NULL,
    wrongAttempts   INT             NOT NULL DEFAULT 0,
    maxAttempts     INT             NOT NULL DEFAULT 6,
    result          VARCHAR(10)     NULL,
    startedAt       DATETIME        NOT NULL DEFAULT GETUTCDATE(),
    finishedAt      DATETIME        NULL
);
GO


CREATE TABLE GuessAttempt (
    attemptId       INT         NOT NULL IDENTITY(1,1),
    sessionId       INT         NOT NULL,
    userId          INT         NOT NULL,
    letter          CHAR(1)     NOT NULL,
    isCorrect       BIT         NOT NULL,
    attemptedAt     DATETIME    NOT NULL DEFAULT GETUTCDATE()
);
GO

-- ==
-- INVITES & CHAT
-- ==


CREATE TABLE Invitation (
    invitationId    INT             NOT NULL IDENTITY(1,1),
    matchId         INT             NOT NULL,
    invitedUserId   INT             NOT NULL,
    invitedBy       INT             NOT NULL,
    [status]          VARCHAR(10)     NOT NULL DEFAULT 'PENDING',
    sentAt          DATETIME        NOT NULL DEFAULT GETUTCDATE(),
    resolvedAt      DATETIME        NULL
);
GO

-- probably not needed anymore

CREATE TABLE [Message] (
    messageId   INT             NOT NULL IDENTITY(1,1),
    matchId     INT             NOT NULL,
    senderId    INT             NOT NULL,
    content     VARCHAR(500)    NOT NULL,
    sentAt      DATETIME        NOT NULL DEFAULT GETUTCDATE(),
    deletedAt   DATETIME        NULL
);
GO