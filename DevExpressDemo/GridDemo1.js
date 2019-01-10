'use strict';

function FileUploadComplete(s, e) {
  if (e.isValid) {
    var data = JSON.parse(e.callbackData);
    var dom = $('#' + data.ClientID).parentsUntil('.dxflNestedControlCell')[1];
    $(dom).find('input[type=text]').val(data.FilePath);
  }
}
