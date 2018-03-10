let MarkerData = [];

const GetMarkerData = $.ajax({
    Method: "Get",
    url: "http://localhost:51208/Markers/GetMarkers"
}).then(function (r) {
    MarkerData = r;
});

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
        console.log(results);
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
                let clat = parseFloat(map.center.lat());
                let clng = parseFloat(map.center.lng());

                //make a new marker with posistion of the coordinates returned
                var marker = new google.maps.Marker({
                    map: map,
                    position: results[0].geometry.location,
                    label: "*"
                });
                MarkerData.forEach(m => {
                    debugger
                    const LatLong = { "lat": parseFloat(m.lat), "lng": parseFloat(m.lng) };
                    let distance = google.maps.geometry.spherical.computeDistanceBetween
                        (new google.maps.LatLng(clat, clng),
                        new google.maps.LatLng(parseFloat(m.lat), parseFloat(m.lng)));
                    console.log(distance);
                   const NewMarker = new google.maps.Marker({
                        map: map,
                        position: LatLong,
                        label: "TestMarker"
                   });
                   debugger
                   let contentString = `
                    <h1>${m.title}</h1>
                    <h1>${m.address}</h1>
                    <img src="${m.image.imageURL} alt="${m.image.imageName} height="100">
                    <p>${m.description}</p>
                    <p>Source: ${m.citation.source}</p>
                    `;

                    var infowindow = new google.maps.InfoWindow({
                       content: contentString
                    });

                    NewMarker.setMap(map);
                    NewMarker.addListener('click', function () {
                        infowindow.open(map, NewMarker);
                   });
                   NewMarker.setMap(map);
                });

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