USE hangmanDB;
GO

-- ==
-- USER
-- ==

CREATE INDEX IX_User_Email
    ON [User](email);
GO

CREATE INDEX IX_User_Username
    ON [User](username);
GO

-- Token validation on recovery flow
CREATE INDEX IX_PasswordRecoveryToken_UserId
    ON PasswordRecoveryToken(userId);
GO

-- Quickly filter out used/expired tokens without scanning the full table
CREATE INDEX IX_PasswordRecoveryToken_IsUsed_ExpiresAt
    ON PasswordRecoveryToken(isUsed, expiresAt);
GO

-- ==
-- ROLES
-- ==

-- Role lookups by user (e.g. checking if a user is admin)
CREATE INDEX IX_UserRole_UserId
    ON UserRole(userId);
GO

-- ==
-- SOCIAL
-- ==

-- GetFriends(userId) — called every time the friends list loads
CREATE INDEX IX_Friendship_UserId
    ON Friendship(userId);
GO

-- Reverse lookup — "is this person my friend"
CREATE INDEX IX_Friendship_FriendId
    ON Friendship(friendId);
GO

-- Inbox of pending requests for a user
CREATE INDEX IX_FriendRequest_ReceiverId_Status
    ON FriendRequest(receiverId, status);
GO

-- Sent requests history
CREATE INDEX IX_FriendRequest_SenderId
    ON FriendRequest(senderId);
GO

-- ==
-- MATCH & GAME
-- ==

-- Lobby browser — list open matches
CREATE INDEX IX_Match_Status
    ON Match(status);
GO

-- "My matches" — host looking up their own matches
CREATE INDEX IX_Match_HostId
    ON Match(hostId);
GO

-- Players in a specific match — called on every lobby load
CREATE INDEX IX_PlayerInMatch_MatchId
    ON PlayerInMatch(matchId);
GO

-- All matches a user has joined
CREATE INDEX IX_PlayerInMatch_UserId
    ON PlayerInMatch(userId);
GO

-- Sessions for a match — core game loop query
CREATE INDEX IX_GameSession_MatchId
    ON GameSession(matchId);
GO

-- Word lookups by difficulty and category (word selection on session start)
CREATE INDEX IX_Word_Difficulty_Category
    ON Word(difficulty, category);
GO

CREATE INDEX IX_Word_Language
    ON Word(language);
GO

-- All guesses for a session — called on every state refresh
CREATE INDEX IX_GuessAttempt_SessionId
    ON GuessAttempt(sessionId);
GO

-- ==
-- INVITE & CHAT
-- ==

-- Pending invitations for a user
CREATE INDEX IX_Invitation_InvitedUserId_Status
    ON Invitation(invitedUserId, status);
GO

-- All invitations for a match
CREATE INDEX IX_Invitation_MatchId
    ON Invitation(matchId);
GO

-- Chat history for a match — ordered by time
CREATE INDEX IX_Message_MatchId_SentAt
    ON Message(matchId, sentAt);
GO

-- ==
-- STATS
-- ==

-- Leaderboard query — order all players by score descending
CREATE INDEX IX_PlayerStats_TotalScore
    ON PlayerStats(totalScore DESC);
GO

-- Friends' stats lookup (UC8 — view befriended users' stats)
CREATE INDEX IX_PlayerStats_UserId
    ON PlayerStats(userId);
GO
