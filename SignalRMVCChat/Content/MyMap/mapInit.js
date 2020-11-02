


window["latT"] =window["addressArr"] && window["addressArr"].length > 0 &&  window["addressArr"][0].latitude ? window["addressArr"][0].latitude : 32.4279;
window["longT"] =window["addressArr"] && window["addressArr"].length > 0 && window["addressArr"][0].longitude ? window["addressArr"][0].longitude : 53.6880;



setTimeout(()=>{
    let mymap = L.map('mapid').setView([window["latT"] ,window["longT"]], 6);

    window['mymap']=mymap;
    L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token=pk.eyJ1IjoibWFwYm94IiwiYSI6ImNpejY4NXVycTA2emYycXBndHRqcmZ3N3gifQ.rJcFIG214AriISLbB6B5aw', {
        maxZoom: 18,
/*        attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors, ' +
            '<a href="https://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, ' +
            'Imagery © <a href="https://www.mapbox.com/">Mapbox</a>',*/
        id: 'mapbox/streets-v11',
        tileSize: 512,
        zoomOffset: -1
    }).addTo(mymap);


    for (let i = 0; i < window["addressArr"].length; i++) {

        let latitude=window["addressArr"][i].latitude;
        let longitude=window["addressArr"][i].longitude;

        //DONT PRINT DUBLICATE
        if (!latitude || !longitude ){
            if (i>0){
                continue;
            }
        }

        latitude=latitude ? latitude : 32.4279;
        longitude=longitude ? longitude : 53.6880;
        
        L.marker([latitude, longitude]).addTo(mymap)
            .bindPopup(`<b>!مکان شناسایی شده کاربر</b>



<br />

<button style="background-color: #1bd8ac;border-radius: 5px 5px 5px 5px;border:none;padding:5px"
 onclick="openChat(${i})">شروع گفتگو</button>
سلام من اینجا هستم


`).openPopup();
        
    }

 
    
    document.getElementById('loading').style.display='none'
},3000)

window['AddMarker']=(latitude, longitude,customer)=>{

    if (!L || ! window['mymap']){
        return ;
    }

    window['customer']=customer;
    latitude=latitude ? latitude : 32.4279;
    longitude=longitude ? longitude : 53.6880;
    
    L.marker([latitude, longitude]).addTo( window['mymap'])
        .bindPopup(`<b>!مکان شناسایی شده کاربر</b>



<br />

<button style="background-color: #1bd8ac;border-radius: 5px 5px 5px 5px;border:none;padding:5px"
 onclick="openChat(${0},'customer')">شروع گفتگو</button>
سلام من اینجا هستم


`).openPopup();
}




function openChat(i,customer) {
    if (customer){
        window["addressArrParent"].onOpenChat(window['customer'])

    }else{
        window["addressArrParent"].onOpenChat(window["addressArr"][i].customer)

    }
}