import React from 'react';
import logo from './logo.svg';
import './App.css';
import MyHeader from './Components/Header';
import Chat from './Components/Chat';
import Customers from './Components/Customers';
import ChatPage from './Pages/ChatPage';
import LayoutPage from './Pages/LayoutPage';
import Menu from "./Components/Menu";

import './styles/myStyle.css'
import './AppGlobalStyle.css'
import FakeServerMonitor from './fakeServer/FakeServerMonitor';
import {MyGlobal} from "./Help/MyGlobal";

import './styles/Tooltip.css'
import 'primereact/resources/themes/saga-blue/theme.css';
import 'primereact/resources/primereact.min.css';
import 'primeicons/primeicons.css';


import 'bootstrap/dist/css/bootstrap.min.css';
import B4AdminLayout from "./External/B4Admin/B4AdminLayout";

function App() {
  return (
    <div className="App">
      
      <B4AdminLayout/>


        {MyGlobal.isTestingEnvirement && <div className="onthefly">
            <FakeServerMonitor></FakeServerMonitor>

        </div>}

      
    </div>
  );
}

export default App;
