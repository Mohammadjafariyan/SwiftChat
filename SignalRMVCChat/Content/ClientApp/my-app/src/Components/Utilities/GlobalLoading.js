import React, { Component } from 'react'
import {Spinner} from 'react-bootstrap';
import {DataHolder} from '../../Help/DataHolder';
import {_GetRandomColor} from './ColorsList';
import {CurrentUserInfo} from '../../Help/Socket';

export let colors=[
    'primary',
    'info',
    'danger',
    'warning',
    'success'
]
export default class GlobalLoading extends Component {
    state={};

    constructor(props) {
        super(props);
        CurrentUserInfo.GlobalLoading=this;
    }
    

    render() {
        let color=colors[Math.floor(Math.random() * colors.length)];
        let loading=DataHolder.loading;
        console.log('GlobalLoading-->DataHolder.loading',loading);
        console.log('GlobalLoading-->color',color);
        return (
            <div>
                 {(this.state.loading || loading || this.props.loading) && (
          <Spinner animation="border" role="status" variant={color}>
            <span className="sr-only">در حال خواندن اطلاعات...</span>
          </Spinner>
        )}
    

       
            </div>
        )
    }
}



export const _SetLoading = (_loo) => {
  if(CurrentUserInfo.GlobalLoading){
    CurrentUserInfo.GlobalLoading.setState({loading:_loo});
  }

  DataHolder.loading=_loo;
}

