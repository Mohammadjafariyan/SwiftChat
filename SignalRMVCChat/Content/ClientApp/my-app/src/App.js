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


function App() {
  return (
    <div className="App">
      
      <LayoutPage/>


        {MyGlobal.isTestingEnvirement && <div className="onthefly">
            <FakeServerMonitor></FakeServerMonitor>

        </div>}

      
    </div>
  );
}

export default App;
