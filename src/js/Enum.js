export const MISAEnum = {
    OptionType: {
        Paging : 1,
        Status: 2,
        Fillter: 3,
        Calculation: 4,
    },
    ActionType:{
        Insert: 1,
        Update: 2,
        Duplicate: 3,
    },
    PopUpType:{
        Confirm: 1,
        Error: 2,
        Alert:3
    },
    AddingType:{
        Unit: 1,
        Stock:2
    },
    Status:{
        Success: 1,
        Error: 2
       },
    KeyBoard:{
        Enter:"Enter",
        ArrowDown:"ArrowDown",
        ArrowUp:"ArrowUp"
    },   
    Date:{
        Day:"Ngày",
        Week:"Tuần",
        Month:"Tháng",
        Year:"Năm"
    },
    Calculation:{
        Multiple: "*",
        Divide:"/",
        MultipleText:"* - Nhân",
        DivideText:"/ - Chia"
    },
   StatusOption:{
    Using: {text: "Không", value: 1},
    NotUsing: {text:"Có",value: 2}
   },
   FillterOption:{
    Contain:{ value: "*", text: "* : Chứa"},
    Equal:{ value: "=", text: "= : Bằng"},
    StartWidth:{value: "+", text: "+ : Bắt đầu bằng"},
    EndWidth:{value: "-", text: "- : Kết thúc bằng"},
    NotContain:{value: "!", text: "! : Không chứa"}
   }
   };