To start this app you need: 

1. Update DefaultConnection in appsettings.json
You need to specify a user, that has admin access rights. You need this, cause when the app starts, It will try to create DB and apply migrations.

2. Update PostingAdmin.Email in appsettings.json
Node - PostingAdmin

3. To start UI you need to install the required packages.
You can do it 2 ways
	* Set Multiple startap projects (MaestroSignant.Api and MaestroSignantUI) and just start app.
	  Visual Studio will manage everything (install packages and start browser instance)
	
	* Set as StartAp only MaestroSignant.Api
	  But then you need to install packages and start UI manually.
	  To do this you need a terminal.
		1. Go to \MaestroSignantApp\MaestroSignantUI\
		2. npm install
		3. npm run serve
		4. open https://localhost:5002/

In case of any questions, feel free to contact me.