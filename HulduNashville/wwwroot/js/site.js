//Array to hold marker data returned from call to the database
let MarkerData = [];
//ajax call to get markers from the database
const GetMarkerData = $.ajax({
    Method: "Get",
    url: "http://localhost:51208/Markers/GetMarkers"
}).then(function (r) {
    //on success, store array in MarkerData
    MarkerData = r;
    });
//function to make new markers. Accepts latlong object, map instacne, and marker object
const makeMarker = function (LatLong, map, m) {
    //Create a new marker with google maps method
    const NewMarker = new google.maps.Marker({
        map: map,
        position: LatLong,
        label: m.title
    });
    //build an html  content string for infowindow that users see when clicking on marker
    let contentString = `
                    <h5>${m.title}</h5>
                    <h5>${m.address}</h5>
                    <img src="${m.image.imageURL} alt="${m.image.imageName} height="100">
                    <p>${m.description}</p>
                    <p>Source: ${m.citation.source}</p>
                    `;
    //create info window
    var infowindow = new google.maps.InfoWindow({
        content: contentString
    });
    //set marker to map
    NewMarker.setMap(map);
    //add listener for click on marker to display info window
    NewMarker.addListener('click', function () {
        infowindow.open(map, NewMarker);
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
        if (status == 'OK') {

            if (results[0].address_components[4].long_name !== "Davidson County") {
                var map = new google.maps.Map(document.getElementById('map'), {
                    zoom: 10,
                    center: { lat: 36.1089409, lng: -86.8468632 }
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
                    makeMarker(MarkersWithDistance[-1].newLatLong, map, MarkersWithDistance[-1]);
                    //extend the bounds of the map view to include the marker
                    bounds.extend(MarkersWithDistance[-1].newLatLong);
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
    };
});