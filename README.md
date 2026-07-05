**Next steps!**

**Storage**
Ive designed the storage to be just a dummy JSON file save and load. This is generally bad for a number of reasons, performance and usability, i have added a {lock} to the file load and save to try and nullify race conditions as that is the main issue with using a file like this. Therefore i would have loved to have the time to have a database or similar, possibly a cosmos db, to save said data but as i was only given an hour or two i figured this would be a good fit for time.
However this could be an easy swap as there are interfaces and dependency injection so aslong as the functions have the same input output, we could swap out the file for a full database.

**Concurrency**
The storage issue i described above is a direct issue for concurrency as this would be the major bottleneck only supporting one read/write at a time where as a database would be able to handle it quicker.
Other than the storage, the main way for the current architecture to handle more users would be to set up the scale out/in options during the pipeline release/bicep/yaml files to make sure we can handle any influx of users and also save money on the low traffic times.

**Security**
This is the main thing that would stop this going anywhere near production. From the looks of the API requested, i would assume that this would be an internal API. Therefore theres a choice between two different options (or both) depending on the use of this API. The first would be to have a JWT token authentication method so that the front end user can log in and use it freely, or if it is more internal have the API on a VPN for ease of use.

**Productionize**
A few things will need doing to productionize the app. App settings for production would need adding, i currently store the path for the JSON storage in the app settings.
Launch settings would need separate files for each environment
And then security as said above

**Swagger**
I know that swagger is no longer the default option for .NET core APIs but i have added it to the API when the environment is "development" for ease of use. Ive also updated the http file for the same reasons

**Improve Architecture/Performance**
The storage is the big impact again but I've said alot about that.
I have currently separated the different layers in a single csproj in different namespaces and folders, as this is just a small API and having one csproj also helps with build times. However if we were to expand and more applications relied on the different layers is to then separate these out into different csproj files.

Another BIG change in architecture i would love to have done if i had the time is to make the viewing booking times less "loose". E.g. every 15 mins for a start of a booking instead of being able to book it at any time like 10:08. This would also open up the possibility of easily checking which "slot" is booked and what is "available" rather than just requesting a time and it would seem more of an intuitive experience"
