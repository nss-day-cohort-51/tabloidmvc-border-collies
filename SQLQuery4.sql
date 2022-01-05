INSERT INTO Comment ( PostId, UserProfileId, Subject, Content, CreateDateTime )
OUTPUT INSERTED.ID
VALUES (1, 1, 'SUBJECT', 'CONTENT', SYSDATETIME());