//function to geocode the address entered by the user and set it at the center of the map
const setCenterMarker = function () {
   
    //get an instance of the geocoder
    let geocoder = new google.maps.Geocoder();

    //get the value of the input field with id "address" and store it in address variable
    const address = $("#address").val();

    //make the call to google maps api to get geocoding of the address
    geocoder.geocode({ 'address': address }, function (results, status) {

        //set the options of the map
        const mapOptions = {
            zoom: 12
        };

        //make a new google map that targets the div with id "map" and pass in the map options
        let map = new google.maps.Map(document.getElementById('map'), mapOptions);

        if (status == 'OK') {
            if (results[0].address_components[4].long_name !== "Davidson County") {
                var map = new google.maps.Map(document.getElementById('map'), {
                    zoom: 10,
                    center: { lat: 36.1089409, lng: -86.8468632 }
                });
                alert("Please Enter An Address in Nashville, Davidson County");
            } else {
            //if the address comes back successfully, set the center of the map to the address passed in
            map.setCenter(results[0].geometry.location);
            //make a new marker with posistion of the coordinates returned
            var marker = new google.maps.Marker({
                map: map,
                position: results[0].geometry.location,
                label: "*"
            });
        }} else {
            //if the address was bad, let the user know why
            alert('Geocode was not successful for the following reason: ' + status);
        }
        //clear the address field
        $("#address").val("");
    })
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