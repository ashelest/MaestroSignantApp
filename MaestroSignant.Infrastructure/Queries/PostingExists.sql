select case
        when per.PersonId is not null then 1
        else 0
       end as PersonExists  
from Posting as pos
inner join Person as per on per.PersonId = pos.PersonId
where per.Email = @Email