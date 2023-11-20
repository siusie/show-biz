# `show-biz`
`show-biz` is a full-stack web application that allows a user to add actors and shows. The database design and view models were built from scratch to practice back-end development skills. The goal was to build a simple application to the best of my current level of knowledge in the ASP.NET MVC framework.

## Example live site
This web app is deployed to Microsoft Azure. It can be accessed [here](https://show-biz.azurewebsites.net/). To create the web application, I used 3 Azure services:

1. A database server
2. SQL database, stored on the database server
3. Web App

### Main layout
![image](https://github.com/siusie/show-biz/assets/93149998/a339a80e-a649-48a0-a1f1-326e836b06ff)

Create an account to start adding actors, shows, and episodes.

### Database design
Persistent storage for the data model contains the following entities:
- Actor
- Show
- Episode
- Genre

The associations between these classes:

![image](https://github.com/siusie/show-biz/assets/93149998/4307d18e-ed56-47af-bdfa-95015d170b00)


### Registering an account
The roles have different "responsibilities".

- `Executive`: Able to add actors
- `Coordinator`: Able to add show(s) for an actor
- `Clerk`: Able to add episode(s) for a show

![image](https://github.com/siusie/show-biz/assets/93149998/f446210c-0ee6-48bb-9254-adc44b5040d1)


### Adding an Actor
The functionality is available to accounts with the "Executive" roles.

![image](https://github.com/siusie/show-biz/assets/93149998/5b318435-2439-4c8d-a0cf-9bf6a902475e)

