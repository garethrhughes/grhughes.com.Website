(function () {
  var sidebar = $('#sidebar'),
              top = sidebar.offset().top;

  $(window).scroll(function () {
    if (document.body.scrollTop > (top - 20)) {
      sidebar.css({ position: 'fixed', top: '20px', width: sidebar.width() });
    } else {
      sidebar.css({ position: 'relative', top: '0', width: 'auto' });
    }
  });

  var mapOptions = {
    center: new google.maps.LatLng(53.3833, -1.4667),
    zoom: 11,
    mapTypeId: google.maps.MapTypeId.ROADMAP,
    disableDefaultUI: true
  };

  new google.maps.Map(document.getElementById("map-canvas"),
              mapOptions);

  $().ready(function () {
    $('#slideshow').embedagram({
      instagram_id: '12485765',
      limit: 12
    });
  });

})();