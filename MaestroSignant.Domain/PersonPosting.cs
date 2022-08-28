namespace MaestroSignant.Domain;

public class PersonPosting
{
    public Guid PostingId { get; set; }
    public Guid AttachmentId { get; set; }

    public Guid PersonId { get; set; }
    public string PersonName { get; set; }
    public string PersonEmail { get; set; }
    public string AttachmentName { get; set; }
    public AttachmentStatus AttachmentStatus { get; set; }

    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}