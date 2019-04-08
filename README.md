# hotelbookingchallenge
A challenge to spike a quick hotel booking api. I have designed this API:
- to support multiple GUI consumers (Web App, Mobile App, etc)
- using 100% Test Driven Development (check commit history) & with SOLID principles in mind
- to adhere to the Async/Await patterns, as in a real-life scenario it would no doubt be doing a lot of DataAccess
- to be stateless, allowing for easy lateral scaling later on

I also used the DotNet CLI to generate the projects initially. 

# Stack
- DotNetCore 
- WebApi
- NUnit (for unit testing)
- Autofac (for Dependancy Injection)
- Swagger/Swashbuckle (for auto-documented API & a post-man like free API GUI) 

# Dependencies 
This project depends on the DotNetCore 2.2 SDK and Visual Studio 2019. It should also build&run with VSCode, but I have not tested this. 

# How to access The Swagger
There is a `launchSettings.json` file commited to the repo. Running the `IIS Express` profile in visual studio should automatically 
launch the api to the desired route for swagger at https://localhost:44356/swagger/index.html

Once it launches, you can use Swagger as a form of auto-documentation or to play with requests/responses of the API. This is hugely
beneficial for teams split into Backend/Frontend teams. Frontend teams can use this early in development to play with the api, similar to PostMan.
Once Auth is introduced, it can also be configured to support auth.

![Swagger](https://imgur.com/Oa88Y3w.jpg)

# How to run the Tests
Open the solution in Visual Studio, right-click the Test project, and click `Run Tests`. 

![All Tests Green](https://imgur.com/EehBf2L.jpg)

# Todo
There are a number of features or design considerations that I would make if this spike were ever to be used in the real world. Here are some of my thoughts:
- logging (I'd suggest NLog with multiple targets, and a global unhandled exception try/catch in `Api.Program.cs` to catch any unhandled exceptions and log. Target Slack or some other chat channel for proactive error investigation.
- identity: Authentication should be introduced, I recommend using Identity Server to support OpenId. This would also support SSO.
- authorization: authorization/security-policies should be introduced as well. I also recommend Identity Server for this, to support OAuth 2.0. 
- scalability: while the api was designed to be stateless, it is not currently configured for lateral scalability. I would introduce Azure Servicefabric for that. 
- containerization: to improve maintainability and deployability, I would normally containerize an API such as this, with Docker. This removes risk of environment differences and allows the api to be migrated to other cloud solutions (AWS, GoogleCloud) more easily.
- db/repository (I would use EFCore w/ SharpCheddar: https://github.com/kriscoleman/SharpCheddar) 
- ordering ability (right now a booking request returns a booking response... 
an order should need to be completed to purchase the room and complete the booking) 
- motel configuration (right now we assume each instance of the api is managing 1 single motel with the same configuration
as defined in the challenge description. However, in real-life it may be better to introduce a collection of motels with their own custom configurations. 
Or make it a level deeper, and create a SaaS by making the api multi-tenanted with tenants owning their own collections of motels)
- room availability (motel should have a listing of available rooms per floor, with a general "Get" method to return a list of all 
available rooms to display in a UI. As orders are completed the availability should be updated.)
- concurrent bookings (right now 2 users trying to book the same kind of room on the same floor could run into a race condition - to solve this, along with room availability,
booking responses should have an expiration of 5 minutes. The room should be reserved for those five minutes and released unless the user completes the order)
- to assist in concurent bookings and ordering, bookingresponses should have a auto-assigned Identity column and be stored in a table.
This Id could be used when completing an order instead of sending the entire BookingResponse back to the API (this would also prevent data manipulation).
- errors should probably be rolled up as an aggregate - if validation fails there could be multiple reasons. 

# The Challenge - Programming Exercise: Booking System

## Problem:

The owner of a motel wants to hire you to build a booking system for them. The motel owner
tells you the following information:
- The motel has two levels.
- Because the second level is only accessible via an outdoor staircase, rooms on this level
are not handicap accessible.
- Rooms can have 1, 2, or 3 beds, at the following nightly costs:
 - 1 bed - $50 per night
 - 2 beds - $75 per night
 - 3 beds - $90 per night
- The motel allows guests to bring pets, but they charge $20 per pet and limit it to 2 pets
maximum. Also, because some pets are messy, pets are only allowed on the ground
floor so that cleaning is easier to perform if necessary.

## Exercise:
Implement a simple booking system based on these requirements.
Technical Notes:
You may completely ignore the database and the UI for this exercise.

## Expectations:
The purpose of this exercise is get a glimpse of how you think about a problem, what design
tradeoffs you consider, and how you translate that design into code. After submitting the
exercise, it will be reviewed by engineers.
There is no set deadline for this exercise, but for the sake of your own time please limit yourself
to a few hours.
You will not be disqualified if your implementation is incomplete or contains flaws. If that is the
case, please include a short summary of your progress so that we know what to expect when
performing the review.
Please provide your solution as a .zip file with a name such as
“firstname_lastname_code_exercise.zip”. You are not expected to provide a front-end or
database solution for this story, only the business logic needed to support it on the backend.
