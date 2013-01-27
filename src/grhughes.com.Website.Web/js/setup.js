(function () {

  var setCookie = function (c_name, value, exdays) {
    var exdate = new Date();
    exdate.setDate(exdate.getDate() + exdays);
    var c_value = escape(value) + ((exdays == null) ? "" : "; expires=" + exdate.toUTCString());
    document.cookie = c_name + "=" + c_value;
  }

  var sidebar = $('#sidebar'),
        top = sidebar.offset().top,
        footer = $('footer'),
        mainContent = $('#main-content'),
        leftContainer = $('div.span3');

  var checkScroll = function () {
    if (sidebar.height() >= mainContent.height()) return;
    if (sidebar.height() >= ($(window).height() - 25)) {
      sidebar.css({ position: 'relative', top: '0', width: 'auto' });
      return;
    }

    var scrollTop = (document.documentElement && document.documentElement.scrollTop) || document.body.scrollTop;
    if (scrollTop > (top - 20)) {
      var height = sidebar.height(),
            footerTop = footer.offset().top;

      if (scrollTop + height > (footerTop - 40)) {
        sidebar.css({ position: 'fixed', top: -(scrollTop + height - footerTop + 20), width: leftContainer.width() });
      } else {
        sidebar.css({ position: 'fixed', top: '20px', width: leftContainer.width() });
      }
    } else {
      sidebar.css({ position: 'relative', top: '0', width: 'auto' });
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

    $('.cookie-dismiss').click(function () {
      setCookie('CookiePolicyAccepted', 1, 365);
    });
  });

  var mapOptions = {
    center: new google.maps.LatLng(53.3833, -1.4667),
    zoom: 11,
    mapTypeId: google.maps.MapTypeId.ROADMAP,
    disableDefaultUI: true
  };

  new google.maps.Map(document.getElementById("map-canvas"),
              mapOptions);

})();