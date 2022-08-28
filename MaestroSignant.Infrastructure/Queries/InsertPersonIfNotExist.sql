begin
   if not exists (select * from Person where Email = @Email)
   begin
       insert into Person 
       values (@PersonId, @Name, @Email, @Phone, @CreatedDate)
   end

   select PersonId from Person where Email = @Email
end