CREATE TRIGGER trCreateUser
ON dbo.AspNetUsers
FOR INSERT
AS
BEGIN
Declare @UserId NvarChar(450)
SELECT @UserId = id from inserted
INSERT INTO dbo.AspNetUserRoles (UserId, RoleId)
VALUES(@UserId, '7957df11-d988-46cd-afd7-bacd6843df8c')
END