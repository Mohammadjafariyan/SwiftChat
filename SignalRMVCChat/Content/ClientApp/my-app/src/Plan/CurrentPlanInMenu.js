import React, {Component} from 'react';
import {DataHolder} from "../Help/DataHolder";
import {CurrentUserInfo} from "../CurrentUserInfo";

class CurrentPlanInMenu extends Component {
    state = {};

    constructor(props) {
        super(props);
        CurrentUserInfo.CurrentPlanInMenu = this;

    }

    getMyProfileCallback(res) {
        if (!res || !res.Content) {
            CurrentUserInfo.LayoutPage.showError('اطلاعات بازگشتی خالی است')
            return
        }

        this.setState({plan: res.Content.plan})

    }

    render() {
        return (
            <>
                <li className="nav-item dropdown no-arrow mx-1" aria-label="ارتقای پلن کاربری"
                    data-microtip-position="left" role="tooltip">
                    <a className="nav-link "
                      onClick={()=>{
                          
                          this.goUpgradePlanPage()
                          
                      }}>
                        {this.state.plan && <small>

                            {this.state.plan.Name}
                        </small>}

                        {!this.state.plan && <small>
                            بسته آزمایشی
                        </small>}

                    </a>

                </li>
            </>
        );
    }

    goUpgradePlanPage() {
        
        
        DataHolder.dashboardUrl="/Customer/UpgradePlan/Index";
        CurrentUserInfo.B4AdminMainMenu.setPage('dashboard');

        if (CurrentUserInfo.DashboardLayout)
        CurrentUserInfo.DashboardLayout.refresh();
        
        this.setState({tm:Math.random()})
        
    }
}

export default CurrentPlanInMenu;