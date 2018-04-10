# HulduNashville

This ASP.NET MVC project was built in the Visual Studio IDE and uses Google Maps API. Client-side users enter an address and see a map containing interactive markers of Nashville Hauntings. The markers are made with data from tables in HulduNashville’s relational database. Users with contributor roles add new marker entires with a form generated via Razor Template. Locations are geocoded during creation of the report, and the lat/long object returned is stored to save on calls to the Google Maps API.

## Registration/Login

These functions are handled with Account Controller (BackEnd/HulduNashville/Controllers/AccountController.cs).  Only one operation has been adjusted from the methods provided automatically when creating a .net MVC with Visual Studio.

Altered Method: Registration.  

Rather then set the username to the email provided, users can provide a user name that is seperate from their email to be displayed when making comments or loggin in.

## Roles

There are two roles that can be assigned, "Admin" & "Contributor."  Users can also use the site anonymously.

The roles are initially created in the database initializer file and then the password for the admin was immediately changed.  Only the admin will see the set roles option in the top nav bar of the home page (BackEnd/HulduNashville/Views/Shared/_Layout.cshtml)

When the "Set Role" option is selected by the admin, drop down menus of users and roles are available in the admin's manage page.  When the admin selects to add or remove a role from a user, the methods in the User Role Controller (BackEnd/HulduNashville/Controllers/UserRolesController.cs).

## Markers

Users with the contributor or admin role will see the "Maker A Marker" option on the home page (BackEnd/HulduNashville/Views/Shared/_Layout.cshtml).

Main functions for creating a new marker are found in the Markers Controller (BackEnd/HulduNashville/Controllers/MarkersController.cs). 
The user will be able to add a title, description, and address that are specific to the Marker.  Categories, sources, and images are used multiple times, and appear in a drop down on the form.
If the user wants to add a new categories, source, or image, forms are available to the right of the main form that take string inputs and reload the Marker razor tempalte with the new data.

Only admin have rights to add images at this time.

When the user adds an address, the string of the address is stored as well as being sent to the google maps API.  The returned lat/long object data is also stored in the Marker Data to prevent multiple calls to google maps for geocoding.

## Main Use

Visitors to the site enter an address, which is sent to the google maps api for geocoding.  If there is an error, or the user has entered a non-Davidson County address, the user is notified.

On successful geocoding in Davidson County, the returned address is compared to the all marker locations.  Markers within 10 miles are populated along with the center marker of the address entered by the user.  If there are no Markers within 10 miles, the closest two are populated. 
(HulduNashville/wwwroot/js/site.js)
