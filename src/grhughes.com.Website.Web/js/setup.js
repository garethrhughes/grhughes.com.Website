(function () {

  $().ready(function () {

    var ele = $('#top-bar > div');

    $(window).scroll(function () {
      var scrollTop = (document.documentElement && document.documentElement.scrollTop) || document.body.scrollTop;
      var para = parseInt(scrollTop / 3.3);

      ele.css({
        '-webkit-transform': 'translate3d(0, ' + para + 'px, 0)',
        '-moz-transform': 'translate3d(0, ' + para + 'px, 0)',
        'transform': 'translate3d(0, ' + para + 'px, 0)'
      });
    });

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