import React, { Component } from 'react'
import { useState } from 'react';
import { MyCaller} from './../Help/Socket';
import { cookieManager } from './../Help/CookieManager';
import { MyGlobal } from '../Help/MyGlobal';
import Chat, {ChatPannel, gotoBottom, showMultimedia} from "../Components/Chat";
import ChatForm from "../Components/ChatForm";
import {DataHolder} from "../Help/DataHolder";
import {CurrentUserInfo} from "../CurrentUserInfo";


