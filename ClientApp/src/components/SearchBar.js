import React, { Component } from 'react';

export class SearchBar extends Component {

    constructor(props) {
        super(props);

        var d = new Date();
        d.setMonth(d.getMonth() - 1);

        this.state = { onChange: props.onChange, searchText: '', searchDate: d };
        this.timeout = null;
    }

    dateValueChanged = event => {
        this.state.searchDate = new Date(event.target.value);
        this.props.onChange(this.state.searchText, this.state.searchDate);
    }

    textValueChanged = event => {
        this.state.searchText = event.target.value;
        
        clearTimeout(this.timeout);
        
        setTimeout(
            () => this.props.onChange(this.state.searchText, this.state.searchDate), 1000);
    }

    render() {
        return (
            <div>
                <div>
                    <input type="text" placeholder="Search" onChange={this.textValueChanged}/>
                </div>
                <div>
                    <label for="publish_date">Data publikacji od: </label>
                    <input type="date" id="publish_date" required="required" onChange={this.dateValueChanged} value={this.state.searchDate.toISOString().substring(0, 10)}/>
                </div>
            </div>
        );
    }

}