select posting.PostingId,
	   posting.AttachmentId,
	   person.PersonId, 
	   person.Name as PersonName, 
	   person.Email as PersonEmail, 
	   posting.AttachmentName, 
	   posting.Status as AttachmentStatus,
	   posting.CreatedDate,
	   posting.ModifiedDate
from Person as person
inner join Posting as posting on person.PersonId = posting.PersonId