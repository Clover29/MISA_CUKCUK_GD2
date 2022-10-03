export const MISAEnum = {
    OptionType: {
        Paging : 1,
        Status: 2,
        Fillter: 3
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