update Posting
set Status = @Status, AttachmentSignedData = @AttachmentSignedData, ModifiedDate = @ModifiedDate
where PostingId = @PostingId