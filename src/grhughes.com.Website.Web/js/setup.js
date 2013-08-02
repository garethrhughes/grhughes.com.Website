﻿(function () {

  $().ready(function () {
    $('#slideshow').embedagram({
      instagram_id: '12485765',
      limit: 12
    });
  });

  var mapOptions = {
    center: new google.maps.LatLng(-33.8674869, 151.2069902),
    zoom: 11,
    mapTypeId: google.maps.MapTypeId.ROADMAP,
    disableDefaultUI: true
  };

  new google.maps.Map(document.getElementById("map-canvas"),
              mapOptions);

})();