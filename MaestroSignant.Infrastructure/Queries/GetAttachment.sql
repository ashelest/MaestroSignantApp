select AttachmentName as Name, ContentType, AttachmentSignedData as Data
from Posting
where PostingId = @PostingId and AttachmentId = @AttachmentId and AttachmentSignedData is not null