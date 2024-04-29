![image](https://github.com/ggrents/asp.net-grenius-api/assets/143025643/ccc30090-26ba-473f-8e56-e68768af6c9c)

# <p align="center">GRENIUS API</p>

## About
Grenius API is an application based on microservice and event-driven architecture, drawing inspiration from the renowned Genius service. It offers users a seamless interface for accessing and managing data about famous artists, their popular songs, lyrics, and annotations. 

### Functionality

The application facilitates retrieval of artist and track data in a user-friendly manner, incorporating pagination and diverse filtering options.
Through designated endpoints, users can seamlessly add and oversee texts and annotations. Utilizing interservice communication, 
the popularity of artists and songs is continually computed, and users can track ratings via dedicated endpoints.

### To enhance the quality, reliability, and performance of the system, the following supplementary development tools have been integrated:
- MS SQL serves as the foundational database for the project, providing structured storage and retrieval of data.
- Redis is utilized for distributed caching, enhancing system performance and scalability by storing frequently accessed data in memory.
- Serilog coupled with Seq is employed for logging purposes, offering sophisticated log management and analysis capabilities, thereby facilitating comprehensive system monitoring and troubleshooting.
- RabbitMQ is chosen as the message broker, providing reliable and asynchronous communication between system components, with MassTransit serving as a facilitative library for seamless integration and interaction with RabbitMQ.
  

  
## Usage examples
- When the application is launched, it is redirected to the swagger page for convenient operation and testing of endpoints

![image](https://github.com/ggrents/grenius-api/assets/143025643/aebef961-44cb-40ed-a1d6-1bbf324f891b)


- Here is a complete list of all endpoints grouped by entity. Each route is provided with a detailed description that will help you understand its functional purpose.

![image](https://github.com/ggrents/grenius-api/assets/143025643/cf38e6ef-631a-48c6-971e-31a91bfb79e9)

![image](https://github.com/ggrents/grenius-api/assets/143025643/2cd724a5-1fd9-42bd-ad63-5f33f3420a5d)

![image](https://github.com/ggrents/grenius-api/assets/143025643/c30d4c15-1359-4435-894c-bf907ba27c4d)


- It can be found in the users section, where the entire functionality of user management can be easily tested. Upon registration, credentials need to be sent to the authentication request, and a JWT token will be received. This token can then be entered in the authorize field, granting permission for operations and data access based on the assigned role.

![image](https://github.com/ggrents/grenius-api/assets/143025643/c86166ab-e17a-4cf1-bfc3-accfd98bc6ca)


- Here are some examples of using the application .

  Fetch list of users with paging parameters
  ![image](https://github.com/ggrents/grenius-api/assets/143025643/18a71c22-63a8-4cab-9221-c13c43610d2c)

  Retrieve a song by ID
  
  ![image](https://github.com/ggrents/grenius-api/assets/143025643/65136e8e-7a45-4a22-970e-9659c8afba74)
  
## Installation and launching the application

```bash
git clone <REPOSITORY_URL>

docker-compose up
```
