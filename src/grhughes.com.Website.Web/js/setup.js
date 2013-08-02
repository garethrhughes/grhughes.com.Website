(function () {

  var sidebar = $('#sidebar'),
      top = 148,
      height = sidebar.height(),
      footer = $('footer'),
      mainContent = $('#main-content'),
      leftContainer = $('div.span3');

  var checkScroll = function () {
    if (sidebar.height() >= mainContent.height()) return;
    if (sidebar.height() >= ($(window).height() - 25)) {
      sidebar.css({ position: 'relative', top: '0', width: 'auto', height: 'auto' });
      return;
    }

    var scrollTop = (document.documentElement && document.documentElement.scrollTop) || document.body.scrollTop;
    if (scrollTop > (top - 20)) {
      var height = sidebar.height(),
          footerTop = footer.offset().top;

      if (scrollTop + height > (footerTop - 40)) {
        sidebar.css({ position: 'fixed', top: -(scrollTop + height - footerTop + 20), width: leftContainer.width(), height: height });
      } else {
        sidebar.css({ position: 'fixed', top: '20px', width: leftContainer.width(), height: height });
      }
    } else {
      sidebar.css({ position: 'relative', top: '0', width: 'auto', height: 'auto' });
    }
  };

  $().ready(function () {
    $(window).scroll(checkScroll);
    $(window).resize(checkScroll);
    checkScroll();

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