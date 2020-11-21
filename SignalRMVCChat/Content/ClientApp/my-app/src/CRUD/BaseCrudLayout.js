import React, {Component} from 'react';
import BaseIndex from "./BaseIndex";

class BaseCrudLayout extends Component {
    state={
        get:'',
        save:'',
        delete:'',
        setIsEnabled:'',
        saveDraft:'',
    };
    constructor(props) {
        super(props);
        
        this.RenderWelcome=this.RenderWelcome.bind(this);
    }

    RenderWelcome(){
        return <p>سلام خوش آمدید</p>
    }
    
    RenderForm(){
        return <></>
    }
    render() {
        return (
            <div>
                
                
                <BaseIndex parent={this} {...this.props} {...this.state} 
                           get={this.state.get} 
                           save={this.state.save} 
                           delete={this.state.delete}
                           setIsEnabled={this.state.setIsEnabled} RenderWelcome={this.RenderWelcome}
                           saveDraft={this.state.saveDraft}>
                    
                    
                    
                    {this.RenderForm()}
                    
                    
                </BaseIndex>
                
            </div>
        );
    }
    
    
    /*====================================== call and callbacks : ======================================*/
    
    getCallback(res){
        this.state.Index.getCallback(res);
    }
    setIsEnabledCallback(res){
        this.state.Index.setIsEnabledCallback(res);
    }

    saveDraftCallback(res){
        this.state.Index.saveDraftCallback(res);
    }

    saveCallback(res){
        this.state.Save.saveCallback(res);
    }

    deleteCallback(res){
        this.state.Save.deleteCallback(res);
    }
    
    
}

export default BaseCrudLayout;