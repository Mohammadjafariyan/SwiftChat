import React, {Component} from 'react';
import BotLogForm from "./BotLogForm";
import BotLogTable from "./BotLogTable";
import BotLogDetail from "./BotLogDetail";
import {_showError} from "../../Pages/LayoutPage";
import {CurrentUserInfo} from "../../CurrentUserInfo";

class BotLogLayout extends Component {
    state = {};

    constructor(props) {
        super(props);
        CurrentUserInfo.BotLogLayout = this;
    }

    getBotLogSingleCallback(res) {
        if (!res || !res.Content) {
            _showError('دیتای بازگشتی نال است')
            return;
        }
        

        let bot=res.Content;
        bot.MutableChildren=bot.children;
        
        bot=this.ConnectChildrenRecursive(bot);
        console.log(bot);
        this.setState({SelectedBotLog: bot});

    }

    ConnectChildrenRecursive(bot) {
        if (!bot.MutableChildren || bot.MutableChildren.length == 0 || bot.MutableChildren.length == null) {
            return bot;
        }
        bot.children = bot.MutableChildren;

        for (let i = 0;
             i < bot.MutableChildren.length;
             i++
        ) {
            bot.MutableChildren[i]=   this.ConnectChildrenRecursive(bot.MutableChildren[i])
        }
        
        return bot;
    }

    render() {
        return (
            <>

                <h3>آمار ربات ها</h3>

                {!this.state.SelectedBotLog &&
                <>

                    <BotLogForm/>


                    <hr/>

                    <BotLogTable/>

                </>}

                {this.state.SelectedBotLog &&
                <>

                    <BotLogDetail parent={this} selectedBot={this.state.SelectedBotLog}/>

                </>}

            </>
        );
    }
}

export default BotLogLayout;