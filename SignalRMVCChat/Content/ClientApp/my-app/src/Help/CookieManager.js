import Cookies from 'universal-cookie';
const cookies = new Cookies();




export const cookieManager = {
    getItem: function (sKey) {
        let val= cookies.get(sKey);
        if (val==='null')
            return;
        return val;
    },
    setItem: function (sKey, sValue, vEnd, sPath, sDomain, bSecure) {
        
        cookies.set(sKey,sValue);
       return true;
    },
    removeItem: function (sKey, sPath, sDomain) {
        cookies.set(sKey,null);
      //  document.cookie = encodeURIComponent(sKey) + "=; expires=Thu, 01 Jan 1970 00:00:00 GMT" + (sDomain ? "; domain=" + sDomain : "") + (sPath ? "; path=" + sPath : "");

        return true;
    },
    hasItem: function (sKey) {
        return (new RegExp("(?:^|;\\s*)" + encodeURIComponent(sKey).replace(/[\-\.\+\*]/g, "\\$&") + "\\s*\\=")).test(document.cookie);
    },
    keys: /* optional method: you can safely remove it! */ function () {
        var aKeys = document.cookie.replace(/((?:^|\s*;)[^\=]+)(?=;|$)|^\s*|\s*(?:\=[^;]*)?(?:\1|$)/g, "").split(/\s*(?:\=[^;]*)?;\s*/);
        for (var nIdx = 0; nIdx < aKeys.length; nIdx++) {
            aKeys[nIdx] = decodeURIComponent(aKeys[nIdx]);
        }
        return aKeys;
    }
};