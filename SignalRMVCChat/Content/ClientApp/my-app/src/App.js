import React from 'react';
import logo from './logo.svg';
import './App.css';
import MyHeader from './Components/Header';
import Chat from './Components/Chat';
import Customers from './Components/Customers';
import ChatPage from './Pages/ChatPage';
import LayoutPage, {MessageAlerts} from './Pages/LayoutPage';
import Menu from "./Components/Menu";

import './styles/myStyle.css'
import './AppGlobalStyle.css'
import FakeServerMonitor from './fakeServer/FakeServerMonitor';
import {MyGlobal} from "./Help/MyGlobal";

import './styles/Tooltip.css'
import 'primereact/resources/themes/saga-blue/theme.css';
import 'primereact/resources/primereact.min.css';
import './styles/primeicons.css';


import 'bootstrap/dist/css/bootstrap.min.css';
import B4AdminLayout from "./External/B4Admin/B4AdminLayout";
import {cookieManager} from "./Help/CookieManager";
import {Toast} from "primereact/toast";
import LoginPage from "./Pages/LoginPage";

function App() {

    let adminToken= cookieManager.getItem("adminToken");
    //console.log(adminToken)


    // console.log(adminToken)

    if (adminToken && adminToken!='null' && adminToken!='undefined') {
        return (
            <div className="App">

                <B4AdminLayout/>


                {MyGlobal.isTestingEnvirement && <div className="onthefly">
                    <FakeServerMonitor></FakeServerMonitor>

                </div>}


            </div>
        );
    }else{

        return (
            <div className="App">

                <div>
                    
                    
                    <LayoutPage/>
                </div>


            </div>
        );
    }
    
    
  
}

export default App;
