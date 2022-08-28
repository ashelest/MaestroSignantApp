select job.Id, job.PostingId, posting.AttachmentId
from PostingSyncJob as job
inner join Posting as posting on posting.PostingId = job.PostingId
where job.SyncStatus = @SyncStatus