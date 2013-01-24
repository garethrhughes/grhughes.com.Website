﻿(function () {
  $().ready(function () {
    var sidebar = $('#sidebar'),
        top = sidebar.offset().top,
        footer = $('footer'),
        mainContent = $('#main-content');


    $(window).scroll(function () {
      if (sidebar.height() >= mainContent.height()) return;

      var scrollTop = (document.documentElement && document.documentElement.scrollTop) || document.body.scrollTop;
      if (scrollTop > (top - 20)) {
        var height = sidebar.height(),
            footerTop = footer.offset().top;

        if (scrollTop + height > (footerTop - 40)) {
          sidebar.css({ position: 'fixed', top: -(scrollTop + height - footerTop + 20), width: sidebar.width() });
        } else {
          sidebar.css({ position: 'fixed', top: '20px', width: sidebar.width() });
        }
      } else {
        sidebar.css({ position: 'relative', top: '0', width: 'auto' });
      }
    });

    $('#slideshow').embedagram({
      instagram_id: '12485765',
      limit: 12
    });

    $('.cookie-dismiss').click(function () {
      setCookie('CookiePolicyAccepted', 1, 365);
    });

    var setCookie = function (c_name, value, exdays) {
      var exdate = new Date();
      exdate.setDate(exdate.getDate() + exdays);
      var c_value = escape(value) + ((exdays == null) ? "" : "; expires=" + exdate.toUTCString());
      document.cookie = c_name + "=" + c_value;
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

})();