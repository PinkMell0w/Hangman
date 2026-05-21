USE hangmanDB;
GO

-- ==
-- PKs
-- ==
ALTER TABLE [User]
	ADD CONSTRAINT PK_User PRIMARY KEY (userId);
GO

ALTER TABLE [Role]
    ADD CONSTRAINT PK_Role PRIMARY KEY (roleId);
GO

ALTER TABLE UserRole
    ADD CONSTRAINT PK_UserRole PRIMARY KEY (userRoleId);
GO

ALTER TABLE PlayerProfile
    ADD CONSTRAINT PK_PlayerProfile PRIMARY KEY (profileId);
GO
ALTER TABLE PlayerStats
    ADD CONSTRAINT PK_PlayerStats PRIMARY KEY (statsId);
GO

ALTER TABLE PasswordRecoveryToken
    ADD CONSTRAINT PK_PasswordRecoveryToken PRIMARY KEY (tokenId);
GO

ALTER TABLE Friendship
    ADD CONSTRAINT PK_Friendship PRIMARY KEY (friendshipId);
GO

ALTER TABLE FriendRequest
    ADD CONSTRAINT PK_FriendRequest PRIMARY KEY (requestId);
GO

ALTER TABLE [Match]
    ADD CONSTRAINT PK_Match PRIMARY KEY (matchId);
GO

ALTER TABLE PlayerInMatch
    ADD CONSTRAINT PK_PlayerInMatch PRIMARY KEY (playerInMatchId);
GO

ALTER TABLE Word
    ADD CONSTRAINT PK_Word PRIMARY KEY (wordId);
GO

ALTER TABLE GameSession
    ADD CONSTRAINT PK_GameSession PRIMARY KEY (sessionId);
GO

ALTER TABLE GuessAttempt
    ADD CONSTRAINT PK_GuessAttempt PRIMARY KEY (attemptId);
GO

ALTER TABLE Invitation
    ADD CONSTRAINT PK_Invitation PRIMARY KEY (invitationId);
GO

ALTER TABLE [Message]
    ADD CONSTRAINT PK_Message PRIMARY KEY (messageId);
GO

-- ================================================
-- UNIQUE CONSTRAINTS
-- ================================================

ALTER TABLE [User]
    ADD CONSTRAINT UQ_User_Username UNIQUE (username);
GO

ALTER TABLE [User]
    ADD CONSTRAINT UQ_User_Email UNIQUE (email);
GO

-- One profile per user
ALTER TABLE PlayerProfile
    ADD CONSTRAINT UQ_PlayerProfile_UserId UNIQUE (userId);
GO

-- One stats row per user
ALTER TABLE PlayerStats
    ADD CONSTRAINT UQ_PlayerStats_UserId UNIQUE (userId);
GO

-- One role assignment per user per role
ALTER TABLE UserRole
    ADD CONSTRAINT UQ_UserRole_User_Role UNIQUE (userId, roleId);
GO

-- Prevent duplicate friendships
ALTER TABLE Friendship
    ADD CONSTRAINT UQ_Friendship_Pair UNIQUE (userId, friendId);
GO

-- Token hash
ALTER TABLE PasswordRecoveryToken
    ADD CONSTRAINT UQ_PasswordRecoveryToken_Token UNIQUE (token);
GO

-- ==
-- FOREIGN KEYS
-- ==

-- UserRole
ALTER TABLE UserRole
    ADD CONSTRAINT FK_UserRole_User
    FOREIGN KEY (userId) REFERENCES [User](userId);
GO

ALTER TABLE UserRole
    ADD CONSTRAINT FK_UserRole_Role
    FOREIGN KEY (roleId) REFERENCES [Role](roleId);
GO

-- PlayerProfile
ALTER TABLE PlayerProfile
    ADD CONSTRAINT FK_PlayerProfile_User
    FOREIGN KEY (userId) REFERENCES [User](userId)
    ON DELETE CASCADE;
GO

-- PlayerStats
ALTER TABLE PlayerStats
    ADD CONSTRAINT FK_PlayerStats_User
    FOREIGN KEY (userId) REFERENCES [User](userId)
    ON DELETE CASCADE;
GO

-- PasswordRecoveryToken
ALTER TABLE PasswordRecoveryToken
    ADD CONSTRAINT FK_PasswordRecoveryToken_User
    FOREIGN KEY (userId) REFERENCES [User](userId)
    ON DELETE CASCADE;
GO

-- Friendship — two FKs to User, only one can CASCADE
ALTER TABLE Friendship
    ADD CONSTRAINT FK_Friendship_User
    FOREIGN KEY (userId) REFERENCES [User](userId)
    ON DELETE CASCADE;
GO

ALTER TABLE Friendship
    ADD CONSTRAINT FK_Friendship_Friend
    FOREIGN KEY (friendId) REFERENCES [User](userId)
    ON DELETE NO ACTION;
GO

-- FriendRequest
ALTER TABLE FriendRequest
    ADD CONSTRAINT FK_FriendRequest_Sender
    FOREIGN KEY (senderId) REFERENCES [User](userId)
    ON DELETE CASCADE;
GO

ALTER TABLE FriendRequest
    ADD CONSTRAINT FK_FriendRequest_Receiver
    FOREIGN KEY (receiverId) REFERENCES [User](userId)
    ON DELETE NO ACTION;
GO

-- Match
ALTER TABLE [Match]
    ADD CONSTRAINT FK_Match_Host
    FOREIGN KEY (hostId) REFERENCES [User](userId);
GO

-- PlayerInMatch
ALTER TABLE PlayerInMatch
    ADD CONSTRAINT FK_PlayerInMatch_Match
    FOREIGN KEY (matchId) REFERENCES [Match](matchId)
    ON DELETE CASCADE;
GO

ALTER TABLE PlayerInMatch
    ADD CONSTRAINT CHK_PlayerInMatch_Role
    CHECK (role IN ('HOST', 'GUESSER'));
GO

ALTER TABLE PlayerInMatch
    ADD CONSTRAINT FK_PlayerInMatch_User
    FOREIGN KEY (userId) REFERENCES [User](userId);
GO

-- Word
ALTER TABLE Word
    ADD CONSTRAINT FK_Word_AddedBy
    FOREIGN KEY (addedBy) REFERENCES [User](userId);
GO

-- GameSession
ALTER TABLE GameSession
    ADD CONSTRAINT FK_GameSession_Match
    FOREIGN KEY (matchId) REFERENCES [Match](matchId)
    ON DELETE CASCADE;
GO

ALTER TABLE GameSession
    ADD CONSTRAINT FK_GameSession_Word
    FOREIGN KEY (wordId) REFERENCES Word(wordId);
GO

ALTER TABLE GameSession
    ADD CONSTRAINT FK_GameSession_Winner
    FOREIGN KEY (winnerId) REFERENCES [User](userId);
GO

-- GuessAttempt
ALTER TABLE GuessAttempt
    ADD CONSTRAINT FK_GuessAttempt_Session
    FOREIGN KEY (sessionId) REFERENCES GameSession(sessionId)
    ON DELETE CASCADE;
GO

ALTER TABLE GuessAttempt
    ADD CONSTRAINT FK_GuessAttempt_User
    FOREIGN KEY (userId) REFERENCES [User](userId);
GO

-- Invitation
ALTER TABLE Invitation
    ADD CONSTRAINT FK_Invitation_Match
    FOREIGN KEY (matchId) REFERENCES [Match](matchId)
    ON DELETE CASCADE;
GO

ALTER TABLE Invitation
    ADD CONSTRAINT FK_Invitation_InvitedUser
    FOREIGN KEY (invitedUserId) REFERENCES [User](userId);
GO

ALTER TABLE Invitation
    ADD CONSTRAINT FK_Invitation_InvitedBy
    FOREIGN KEY (invitedBy) REFERENCES [User](userId);
GO

-- Message
ALTER TABLE Message
    ADD CONSTRAINT FK_Message_Match
    FOREIGN KEY (matchId) REFERENCES [Match](matchId)
    ON DELETE CASCADE;
GO

ALTER TABLE Message
    ADD CONSTRAINT FK_Message_Sender
    FOREIGN KEY (senderId) REFERENCES [User](userId);
GO

-- ==
-- CHECK CONSTRAINTS
-- ==

ALTER TABLE [Role]
    ADD CONSTRAINT CHK_Role_RoleName
    CHECK (roleName IN ('ADMIN', 'PLAYER'));
GO

ALTER TABLE FriendRequest
    ADD CONSTRAINT CHK_FriendRequest_Status
    CHECK ([status] IN ('PENDING', 'ACCEPTED', 'DECLINED'));
GO

-- Prevent a user from sending themselves a friend request
ALTER TABLE FriendRequest
    ADD CONSTRAINT CHK_FriendRequest_NoSelfRequest
    CHECK (senderId <> receiverId);
GO

ALTER TABLE [Match]
    ADD CONSTRAINT CHK_Match_Status
    CHECK ([status] IN ('WAITING', 'IN_PROGRESS', 'FINISHED', 'CANCELLED'));
GO

ALTER TABLE [Match]
    ADD CONSTRAINT CHK_Match_MaxPlayers
    CHECK (maxPlayers BETWEEN 2 AND 8);
GO

ALTER TABLE Word
    ADD CONSTRAINT CHK_Word_Difficulty
    CHECK (difficulty IN ('EASY', 'MEDIUM', 'HARD'));
GO

ALTER TABLE GameSession
    ADD CONSTRAINT CHK_GameSession_Result
    CHECK (result IN ('WIN', 'LOSS', 'SURRENDER'));
GO

ALTER TABLE Invitation
    ADD CONSTRAINT CHK_Invitation_Status
    CHECK ([status] IN ('PENDING', 'ACCEPTED', 'EXPIRED'));
GO

ALTER TABLE GuessAttempt
    ADD CONSTRAINT CHK_GuessAttempt_Letter
    CHECK (letter LIKE '[A-Za-z]');
GO