Begin transaction

	CREATE TABLE [Person] (
		PersonId		uniqueidentifier NOT NULL,

		Name			nvarchar(max)	 NOT NULL,
		Email			nvarchar(max)	 NOT NULL,
		MobileNumber	nvarchar(max)	 NULL,

		CreatedDate		datetime		 NOT NULL,

		CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED ([PersonId] ASC)
	);
	GO

	CREATE TABLE [Posting] (
		PostingId				uniqueidentifier NOT NULL,
		AttachmentId			uniqueidentifier NOT NULL,

		PersonId				uniqueidentifier NOT NULL,
		
		ContentType				nvarchar(max)	 NOT NULL,
		AttachmentName			nvarchar(max)	 NOT NULL,
		AttachmentOriginalData  varBinary(max)   NOT NULL,
		AttachmentSignedData    varBinary(max)   NULL,

		Status					nvarchar(max)	 NOT NULL,

		CreatedDate		datetime		NOT NULL,
		ModifiedDate	datetime		NULL,

		CONSTRAINT [PK_PostingId] PRIMARY KEY CLUSTERED ([PostingId] ASC),
		CONSTRAINT [FK_Posting_Person] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Person]([PersonId])
	);
	GO

	CREATE TABLE [PostingSyncJob] (
		Id	uniqueidentifier NOT NULL,
		SyncStatus			nvarchar(max)	NOT NULL ,
		
		PostingId			uniqueidentifier NOT NULL,

		CreatedDate		datetime		NOT NULL,
		ModifiedDate	datetime		NULL,

		CONSTRAINT [PK_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
		CONSTRAINT [FK_PostingSyncJob_Posting] FOREIGN KEY ([PostingId]) REFERENCES [dbo].[Posting]([PostingId])
	);
	GO

Commit transaction
