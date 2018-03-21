//Array to hold marker data returned from call to the database
let MarkerData = [];
//ajax call to get markers from the database
let Comments = [];
const GetMarkerData = $.ajax({
    Method: "Get",
    url: "/Markers/GetMarkers"
}).then(function (r) {
    //on success, store array in MarkerData
    MarkerData = r;
    });
const GetComments = $.ajax({
    Method: "Get",
    url: "Comments/GetComments"
}).then(function (r) {
    //on success, store array in MarkerData
    Comments = r;
});
//function to make new markers. Accepts latlong object, map instacne, and marker object
const makeMarker = function (LatLong, map, m) {
    //Create a new marker with google maps method
    const NewMarker = new google.maps.Marker({
        map: map,
        position: LatLong,
        label: m.title
    });
    //filter out comments that match the marker
    let MarkerComments = Comments.filter(c => c.markerId === m.id).sort((a, b) => b.commentId - a.commentId);
    //build an html  content string for infowindow that users see when clicking on marker
    let contentString = `
                    <div class="infoWindowContainer">
                    <h5>${m.title}</h5>
                    <h5>${m.address}</h5>
                    <p class="disclaimer">***DO NOT VISIT ANY PROPERTY WITHOUT PERMSSION OF OWNER***</p>
                    <div class="infoWindowPicDiv">
                    <img src="${m.image.imageURL}" alt="${m.image.imageName}">
                    </div>
                    <p>${m.description}</p>
                    `;
    if (m.citation.source.includes("http")) {
        contentString += `
                        <a href="${m.citation.source}" target="_blank">Source: ${m.citation.source}</a>
                        `;
    } else {
        contentString += `<p>Source: ${m.citation.source}</p>
    `};

    contentString += `
                       <div class="commentsDiv">
                       <h5>Comments</h5>
                       `;
    MarkerComments.forEach(mc => {
        contentString += `
            <p class="commentsDiv_User">${mc.userName} says: </p>
            <p id=${mc.markerId} class="comment">${mc.commentString}</p>
        ` 
    });
    contentString += `
                       </div>
                       <button class="addComment" id="${m.id}">Add Comment</button>
                       </div>
                       `;
    //create info window
    var infowindow = new google.maps.InfoWindow({
        content: contentString,
        maxWidth: 500,
        maxHeight: 200
    });
    //set marker to map
    NewMarker.setMap(map);
    //add listener for click on marker to display info window
    NewMarker.addListener('click', function () {
        $("#map").css("height", "100vh");
        infowindow.open(map, NewMarker);
        //add listener for click outside of marker to close window and recenter map
        google.maps.event.addListener(map, "click", function (event) {
            infowindow.close();
            $("#map").css("height", "30em");
            map.setCenter(LatLong);
        });
        //add listener for click on close infowindow button to reset map
        google.maps.event.addListener(infowindow, 'closeclick', function () {
            $("#map").css("height", "30em");
            map.setCenter(LatLong);
        });
    });
};

//function to geocode the address entered by the user and set it at the center of the map
const setCenterMarker = function () {

    //set the options of the map
    const mapOptions = {
        zoom: 12
    };

    //make a new google map that targets the div with id "map" and pass in the map options
    let map = new google.maps.Map(document.getElementById('map'), mapOptions);

    //get an instance of the geocoder
    let geocoder = new google.maps.Geocoder();

    //get the value of the input field with id "address" and store it in address variable
    const address = $("#address").val();
    
    //make the call to google maps api to get geocoding of the address
    geocoder.geocode({ 'address': address }, function (results, status) {
        const bounds = new google.maps.LatLngBounds();
        if (status === 'OK') {

            if (results[0].address_components[4].long_name !== "Davidson County") {
                var map = new google.maps.Map(document.getElementById('map'), {
                    zoom: 12,
                    center: results[0].geometry.location
                });
                var marker = new google.maps.Marker({
                    map: map,
                    position: results[0].geometry.location,
                    label: "*"
                });
                alert("Please Enter An Address in Nashville, Davidson County");
            } else {
                //if the address comes back successfully, set the center of the map to the address passed in
                var map = new google.maps.Map(document.getElementById('map'), {
                    zoom: 12,
                    center: results[0].geometry.location
                });

                //lat and long coordinates of the map center
                let clat = parseFloat(map.center.lat());
                let clng = parseFloat(map.center.lng());
                //extend the bounds of the map view to include the center
                bounds.extend({ "lat": clat, "lng": clng });

                //make a new marker with posistion of the coordinates returned
                var marker = new google.maps.Marker({
                    map: map,
                    position: results[0].geometry.location,
                    label: "*"
                });
                //Array to hold Markers with distance from center
                const MarkersWithDistance = [];
                //For each marker returned from the database...
                MarkerData.forEach(m => {
                    //create a lat/long object to pass to googlemaps api with marker coordinates
                    const LatLong = { "lat": parseFloat(m.lat), "lng": parseFloat(m.lng) };
                    //calculate the distance between the center of the map and the marker
                    let distance = google.maps.geometry.spherical.computeDistanceBetween
                        (new google.maps.LatLng(clat, clng),
                        new google.maps.LatLng(parseFloat(m.lat), parseFloat(m.lng)));
                    //if the distance is less than 10 miles(16093.3 meters)...
                    if (distance < 16093.4) {
                        //call the makeMarker method. Pass coordinates, map, and marker info
                        makeMarker(LatLong, map, m);
                        //extend the bounds of the map view to include the marker
                        bounds.extend(LatLong);
                    } else {
                        //if over 10 miles, add the distance and latlong object to the marker
                        m.distance = distance;
                        m.newLatLong = LatLong;
                        //push the updated marker to array
                        MarkersWithDistance.push(m);
                    }
                });
                /*If the MarkerData array is the same length as the MarkersWithDistance array
                then none of the markers were within 10 miles of the center, so sort MarkersWithDistance
                by distance and return the closest one*/
                if (MarkersWithDistance.length === MarkerData.length) {
                    MarkersWithDistance.sort((a, b) => a.distance - b.distance);
                    makeMarker(MarkersWithDistance[MarkersWithDistance.length - 1].newLatLong, map, MarkersWithDistance[MarkersWithDistance.length-1]);
                    //extend the bounds of the map view to include the marker
                    bounds.extend(MarkersWithDistance[MarkersWithDistance.length-1].newLatLong);
                }
                //center the map to the geometric center of all markers
                map.setCenter(bounds.getCenter());
                map.fitBounds(bounds);
                //remove one zoom level to ensure no marker is on the edge.
                map.setZoom(map.getZoom() - 1);
            }
        } else {
            //if the address was bad, let the user know why
            alert('Geocode was not successful for the following reason: ' + status);
        }
        //clear the address field
        $("#address").val("");
    });
};

//listen for click on the submit button.  On click, call setCenterMarker function
$("#submit").on("click", setCenterMarker);
//listen for press of enter key in input field
$("#address").on("keyup", function (e) {
    if (e.keyCode === 13) {
        //if the user presses enter, call setCenterMarker()
        setCenterMarker();
    }
});